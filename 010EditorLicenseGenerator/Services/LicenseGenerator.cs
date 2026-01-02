namespace EditorLicenseGenerator.Services;

/// <summary>
/// Interface for license generation operations
/// </summary>
public interface ILicenseGenerator
{
    /// <summary>
    /// Generate Time License (0xAC) - expires after specified days
    /// </summary>
    string GenerateTimeLicense(string name, int numUsers, int daysLeft);
    
    /// <summary>
    /// Generate Version License (0x9C) - version-specific license
    /// </summary>
    string GenerateVersionLicense(string name, int numUsers, int version);
    
    /// <summary>
    /// Generate Trial License (0xFC) - trial license
    /// </summary>
    string GenerateTrialLicense(string name, int offset = 365);
}

/// <summary>
/// License generator implementation ported from Python
/// </summary>
public class LicenseGenerator : ILicenseGenerator
{
    // Raw lookup table from Python implementation
    private static readonly uint[] Raw = new uint[]
    {
        969622712, 594890599, 1593930257, 1052452058, 890701766, 1677293387, 394424968, 266815521,
        1532978959, 1211194088, 2019260265, 729421127, 953225874, 1117854514, 892543556, 2000911200,
        514538256, 1400963072, 486675118, 1862498216, 1136668818, 758909582, 1653935295, 821063674,
        888606944, 687085563, 890056597, 1513495898, 365692427, 184357836, 677395407, 863045227,
        818746596, 391985767, 1842768403, 758385145, 1478392706, 1985112985, 1552765320, 746944881,
        368385984, 1758203153, 1240817244, 660489060, 756944316, 1290697955, 844453952, 288239112,
        1769473626, 1922176006, 826636519, 391520695, 1081548223, 1069693142, 1244729994, 766313326,
        1101031894, 624951698, 14501479, 1794907983, 1460682958, 1660839647, 1104890686, 897721119,
        1442187162, 480708164, 454443986, 1064446153, 1595150448, 1041527979, 1145775470, 1399869657,
        255985995, 802693350, 2005610078, 1897360642, 2146073193, 1538606632, 431647857, 964049561,
        395138253, 19164808, 856904574, 730737943, 708645054, 1506870658, 933323739, 819349658,
        1780571206, 236747382, 533160167, 2042104933, 670325172, 2040165158, 1354372994, 705785180,
        1669754395, 1066536508, 1426207888, 1437950089, 741941201, 796931522, 1694313338, 1290302874,
        1367672048, 2039808424, 1062939821, 954597728, 1668694488, 859122242, 1369582617, 140269649,
        53024683, 729221831, 816609203, 736893191, 55706320, 262747091, 1629838835, 581764799,
        1488480625, 1607077349, 1879925846, 1453945819, 1521965565, 856558562, 1530662365, 1230847072,
        1404918182, 1281256849, 1238970765, 272453753, 1640907491, 2127893021, 350314733, 556617458,
        654390256, 1648581270, 531062411, 1862873022, 1241517385, 1471028336, 5121143, 1444839026,
        1183580211, 1573659650, 2018540230, 1487873223, 234237236, 898254600, 1023090193, 728843548,
        2007454357, 1451820833, 267351539, 302982385, 26807015, 865879122, 664886158, 195503981,
        1625037691, 1330347906, 1742434311, 1330272217, 1645368040, 542321916, 1782121222, 411042851,
        435386250, 1176704752, 1454246199, 1136813916, 1707755005, 224415730, 201138891, 989750331,
        1006010278, 1147286905, 406860280, 840388503, 1282017578, 1605698145, 23396724, 862145265,
        1898780916, 1855549801, 1571519230, 2083204840, 1859876276, 1602449334, 1009413590, 690816450,
        86131931, 345661263, 1565025600, 857544170, 1329948960, 1211787679, 994381573, 991984748,
        1956475134, 1098146294, 1655714289, 659576699, 689116467, 1485584392, 451884118, 255590636,
        2108114754, 1266252396, 1589326471, 2019907768, 15552498, 1651075358, 614606175, 1656823678,
        797605325, 1681594366, 2005080248, 624648446, 884695971, 1526931791, 1595240948, 439447199,
        2060396292, 680093752, 409028215, 469068267, 195583689, 1791650630, 507724330, 1364025102,
        1094582668, 813049577, 32316922, 1240756058, 1176200235, 2104494066, 325396055, 1796606917,
        1709197385, 525495836, 1510101430, 735526761, 767523533, 1374043776, 1559389967, 567085571,
        1560216161, 867042846, 1001796703, 1568754293, 628841972, 173812827, 379868455, 384973125
    };

    /// <summary>
    /// Decode uses left value
    /// </summary>
    private static int DecodeUsesLeft(uint n)
    {
        unchecked
        {
            n = (n ^ 0x7328b47a) - 0x18b3c906 ^ 0xbf32abce;
            if (n % 1179 == 0)
                return (int)(n / 1179);
            return 0;
        }
    }

    /// <summary>
    /// Encode uses left value
    /// </summary>
    private static uint EncodeUsesLeft(int n)
    {
        unchecked
        {
            return (uint)((n * 1179) ^ 0xbf32abce) + 0x18b3c906 ^ 0x7328b47a;
        }
    }

    /// <summary>
    /// Encode name for license generation
    /// </summary>
    private static uint EncodeName(string name, bool isNotFcLicense, int left, int nUsers)
    {
        unchecked
        {
            uint ans = 0;
            int leftVal = left * 17;
            int nUsersVal = nUsers * 15;
            int x = 0;
            int y = 0;

            foreach (char ch in name)
            {
                int charVal = char.ToUpper(ch);
                ans += Raw[charVal];
                
                if (isNotFcLicense)
                {
                    ans ^= Raw[(charVal + 13) & 0xff];
                    ans *= Raw[(charVal + 0x2f) & 0xff];
                    ans += Raw[x & 0xff];
                }
                else
                {
                    ans ^= Raw[(charVal + 0x3f) & 0xff];
                    ans *= Raw[(charVal + 0x17) & 0xff];
                    ans += Raw[y & 0xff];
                }
                
                ans += Raw[leftVal & 0xff] + Raw[nUsersVal & 0xff];
                x += 19;
                y += 7;
                leftVal += 9;
                nUsersVal += 13;
            }
            
            return ans;
        }
    }

    /// <summary>
    /// Encode number of users
    /// </summary>
    private static uint EncodeUsers(int num)
    {
        unchecked
        {
            return (uint)(((num * 11) ^ 0x3421) - 0x4d30) ^ 0x7892;
        }
    }

    /// <summary>
    /// Encode password date
    /// </summary>
    private static uint EncodePasswordDate(int a, uint b)
    {
        unchecked
        {
            return (uint)(((a * 17) ^ 0xa8e53167) + 0x2c175) ^ 0xff22c078 ^ b;
        }
    }

    /// <summary>
    /// Format license key from byte array
    /// </summary>
    private static string FormatLicense(byte[] p)
    {
        var parts = new List<string>();
        
        for (int i = 0; i < p.Length; i += 2)
        {
            int tmp = (p[i] << 8) | p[i + 1];
            parts.Add($"{tmp & 0xffff:X4}");
        }
        
        return string.Join("-", parts);
    }

    /// <summary>
    /// Generate Version License (0x9C)
    /// </summary>
    public string GenerateVersionLicense(string name, int numUsers, int version)
    {
        unchecked
        {
            byte[] p = new byte[8];
            p[3] = 0x9c;
            
            uint csum = EncodeName(name, true, 0, numUsers);
            p[4] = (byte)(csum & 0xff);
            p[5] = (byte)((csum >> 8) & 0xff);
            p[6] = (byte)((csum >> 16) & 0xff);
            p[7] = (byte)((csum >> 24) & 0xff);
            
            uint t = EncodeUsers(numUsers);
            p[2] = (byte)(p[5] ^ (t & 0xff));
            p[1] = (byte)(p[7] ^ ((t >> 8) & 0xff));
            
            int tVersion = ((version ^ 0xa7) - 0x3d) ^ 0x18; // p[0]^p[6]
            p[0] = (byte)(tVersion ^ p[6]);
            
            return FormatLicense(p);
        }
    }

    /// <summary>
    /// Generate Trial License (0xFC)
    /// </summary>
    public string GenerateTrialLicense(string name, int offset = 365)
    {
        unchecked
        {
            byte[] p = new byte[8];
            p[3] = 0xfc;
            
            uint csum = EncodeName(name, false, 0xff, 1);
            p[4] = (byte)(csum & 0xff);
            p[5] = (byte)((csum >> 8) & 0xff);
            p[6] = (byte)((csum >> 16) & 0xff);
            p[7] = (byte)((csum >> 24) & 0xff);
            
            uint temp = EncodePasswordDate(offset, csum);
            p[0] = (byte)(temp & 0xff);
            p[1] = (byte)((temp >> 8) & 0xff);
            p[2] = (byte)((temp >> 16) & 0xff);
            
            return FormatLicense(p);
        }
    }

    /// <summary>
    /// Generate Time License (0xAC)
    /// </summary>
    public string GenerateTimeLicense(string name, int numUsers, int daysLeft)
    {
        unchecked
        {
            byte[] p = new byte[10];
            p[3] = 0xac;
            
            int adjustedDays = daysLeft + 0x4596 + 83;
            uint csum = EncodeName(name, true, adjustedDays, numUsers);
            p[4] = (byte)(csum & 0xff);
            p[5] = (byte)((csum >> 8) & 0xff);
            p[6] = (byte)((csum >> 16) & 0xff);
            p[7] = (byte)((csum >> 24) & 0xff);
            
            uint encodedDate = EncodePasswordDate(adjustedDays, 0x5b8c27);
            uint encUsers = EncodeUsers(numUsers);
            
            p[2] = (byte)(p[5] ^ (encUsers & 0xff));
            p[1] = (byte)(p[7] ^ ((encUsers >> 8) & 0xff));
            p[0] = (byte)(p[6] ^ (encodedDate & 0xff));
            p[8] = (byte)(p[4] ^ ((encodedDate >> 8) & 0xff));
            p[9] = (byte)(p[5] ^ ((encodedDate >> 16) & 0xff));
            
            return FormatLicense(p);
        }
    }
}
