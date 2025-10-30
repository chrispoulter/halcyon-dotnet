import { ArrowDownWideNarrow } from 'lucide-react';
import type { UserSort } from '@/features/user/hooks/use-search-users';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';

type SortUsersDropdownOption = { value: UserSort; label: string };

const sortOptions: SortUsersDropdownOption[] = [
    {
        value: 'NAME_ASC',
        label: 'Name A-Z',
    },
    {
        value: 'NAME_DESC',
        label: 'Name Z-A',
    },
    {
        value: 'EMAIL_ADDRESS_ASC',
        label: 'Email Address A-Z',
    },
    {
        value: 'EMAIL_ADDRESS_DESC',
        label: 'Email Address Z-A',
    },
];

type SortUsersDropdownProps = {
    sort?: UserSort;
    onChange: (sort: UserSort) => void;
    disabled?: boolean;
};

export function SortUsersDropdown({
    sort,
    onChange,
    disabled,
}: SortUsersDropdownProps) {
    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button variant="secondary" size="icon" disabled={disabled}>
                    <ArrowDownWideNarrow />
                    <span className="sr-only">Toggle sort</span>
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56">
                {sortOptions.map(({ label, value }) => (
                    <DropdownMenuItem
                        key={value}
                        disabled={sort === value}
                        onClick={() => onChange(value)}
                    >
                        {label}
                    </DropdownMenuItem>
                ))}
            </DropdownMenuContent>
        </DropdownMenu>
    );
}
