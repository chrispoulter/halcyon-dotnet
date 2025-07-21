import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import type {
    SearchUsersRequest,
    SearchUsersResponse,
} from '@/features/user/user-types';
import { apiClient } from '@/lib/api-client';

export const useSearchUsers = (request: SearchUsersRequest) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['users', request],
        queryFn: ({ signal }) =>
            apiClient.get<SearchUsersResponse>('/user', signal, request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        placeholderData: keepPreviousData,
    });
};
