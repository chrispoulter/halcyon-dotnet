import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';

export type UserSort =
    | 'EMAIL_ADDRESS_ASC'
    | 'EMAIL_ADDRESS_DESC'
    | 'NAME_ASC'
    | 'NAME_DESC';

type SearchUsersRequest = {
    search?: string;
    sort: UserSort;
    page: number;
    size: number;
};

export type SearchUsersResponse = {
    items: {
        id: string;
        emailAddress: string;
        firstName: string;
        lastName: string;
        isLockedOut?: boolean;
        roles?: Role[];
    }[];
    hasNextPage: boolean;
    hasPreviousPage: boolean;
};

export const useSearchUsers = (request: SearchUsersRequest) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['users', request],
        queryFn: ({ signal }) =>
            apiClient
                .get('users', {
                    searchParams: request,
                    context: {
                        accessToken,
                    },
                    signal,
                })
                .json<SearchUsersResponse>(),
        placeholderData: keepPreviousData,
    });
};
