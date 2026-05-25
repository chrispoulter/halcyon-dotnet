import {
    keepPreviousData,
    useMutation,
    useQuery,
    useQueryClient,
} from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';
import { profileKeys } from '../profile/profile-queries';

export const userKeys = {
    all: ['users'] as const,
    search: (request: SearchUsersRequest) => ['users', request] as const,
    detail: (id: string) => ['user', id] as const,
};

export interface GetUserResponse {
    id: string;
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    isLockedOut?: boolean;
    roles?: Role[];
}

export const useGetUser = (id: string) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: userKeys.detail(id),
        queryFn: ({ signal }) =>
            apiClient
                .get(`users/${id}`, {
                    context: {
                        accessToken,
                    },
                    signal,
                })
                .json<GetUserResponse>(),
    });
};

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

export interface SearchUsersResponse {
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
}

export const useSearchUsers = (request: SearchUsersRequest) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: userKeys.search(request),
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

interface CreateUserRequest {
    emailAddress: string;
    password: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    roles?: Role[];
}

interface CreateUserResponse {
    id: string;
}

export const useCreateUser = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: CreateUserRequest) =>
            apiClient
                .post('users', {
                    json: request,
                    context: {
                        accessToken,
                    },
                })
                .json<CreateUserResponse>(),
        onSuccess: () =>
            queryClient.invalidateQueries({ queryKey: userKeys.all }),
    });
};

interface UpdateUserRequest {
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    roles?: Role[];
}

interface UpdateUserResponse {
    id: string;
}

export const useUpdateUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: UpdateUserRequest) =>
            apiClient
                .put(`users/${id}`, {
                    json: request,
                    context: {
                        accessToken,
                    },
                })
                .json<UpdateUserResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: userKeys.all });
            queryClient.invalidateQueries({
                queryKey: userKeys.detail(data.id),
            });
        },
    });
};

interface DeleteUserResponse {
    id: string;
}

export const useDeleteUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient
                .delete(`users/${id}`, {
                    context: {
                        accessToken,
                    },
                })
                .json<DeleteUserResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: userKeys.all });

            queryClient.invalidateQueries({
                queryKey: profileKeys.all,
                refetchType: 'none',
            });

            queryClient.invalidateQueries({
                queryKey: userKeys.detail(data.id),
                refetchType: 'none',
            });
        },
    });
};

interface LockUserResponse {
    id: string;
}

export const useLockUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient
                .put(`users/${id}/lock`, {
                    context: {
                        accessToken,
                    },
                })
                .json<LockUserResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: profileKeys.all });
            queryClient.invalidateQueries({ queryKey: userKeys.all });
            queryClient.invalidateQueries({
                queryKey: userKeys.detail(data.id),
            });
        },
    });
};

interface UnlockUserResponse {
    id: string;
}

export const useUnlockUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient
                .put(`users/${id}/unlock`, {
                    context: {
                        accessToken,
                    },
                })
                .json<UnlockUserResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: profileKeys.all });
            queryClient.invalidateQueries({ queryKey: userKeys.all });
            queryClient.invalidateQueries({
                queryKey: userKeys.detail(data.id),
            });
        },
    });
};
