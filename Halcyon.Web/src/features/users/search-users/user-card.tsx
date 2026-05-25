import { Link } from 'react-router';
import { Badge } from '@/components/ui/badge';
import { roleOptions } from '@/lib/session';
import type { SearchUsersResponse } from '../users-queries';

interface UserCardProps {
    user: SearchUsersResponse['items'][number];
}

export function UserCard({ user }: UserCardProps) {
    return (
        <Link
            to={`/users/${user.id}`}
            className="block space-y-2 rounded-lg border p-4 focus-within:bg-accent hover:bg-accent"
        >
            <div className="space-y-0.5">
                <div className="truncate text-base font-medium">
                    {user.firstName} {user.lastName}
                </div>
                <div className="truncate text-sm text-muted-foreground">
                    {user.emailAddress}
                </div>
            </div>
            <div className="flex flex-col gap-2 sm:flex-row">
                {user.isLockedOut && (
                    <Badge variant="destructive" className="w-full sm:w-auto">
                        Locked
                    </Badge>
                )}
                {user.roles?.map((role) => (
                    <Badge
                        key={role}
                        variant="secondary"
                        className="w-full sm:w-auto"
                    >
                        {roleOptions[role].title}
                    </Badge>
                ))}
            </div>
        </Link>
    );
}
