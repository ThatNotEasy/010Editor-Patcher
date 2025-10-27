import argparse
from modules.patcher import generate_license_ac, generate_license_9c, generate_license_fc
from modules.banners import clear_and_print


def main():
    parser = argparse.ArgumentParser(
        prog='010Editor License Generator',
        description='Generate different types of licenses for 010Editor'
    )

    subparsers = parser.add_subparsers(dest='license_type', required=True, help='License type to generate')

    # Time License
    parser_time = subparsers.add_parser('time', help='Generate Time License (ac)')
    parser_time.add_argument('name', help='User name')
    parser_time.add_argument('users', type=int, help='Number of users')
    parser_time.add_argument('days', type=int, help='Days left')

    # Version License
    parser_version = subparsers.add_parser('version', help='Generate Version License (9c)')
    parser_version.add_argument('name', help='User name')
    parser_version.add_argument('users', type=int, help='Number of users')
    parser_version.add_argument('version', type=int, help='Version number')

    # Trial License
    parser_trial = subparsers.add_parser('trial', help='Generate Trial License (fc)')
    parser_trial.add_argument('name', help='User name')

    args = parser.parse_args()

    if args.license_type == 'time':
        print(generate_license_ac(args.name, args.users, args.days))
    elif args.license_type == 'version':
        print(generate_license_9c(args.name, args.users, args.version))
    elif args.license_type == 'trial':
        print(generate_license_fc(args.name))


if __name__ == '__main__':
    clear_and_print()
    main()
