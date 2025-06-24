export type Role = 'SYSTEM_ADMINISTRATOR' | 'USER_ADMINISTRATOR';

export const Role = {
    SYSTEM_ADMINISTRATOR: 'SYSTEM_ADMINISTRATOR' as Role,
    USER_ADMINISTRATOR: 'USER_ADMINISTRATOR' as Role,
};

export const roles = {
    [Role.SYSTEM_ADMINISTRATOR]: {
        title: 'System Administrator',
        description: 'A system administrator has access to the entire system.',
    },
    [Role.USER_ADMINISTRATOR]: {
        title: 'User Administrator',
        description: 'A user administrator can create / update / delete users.',
    },
};

export type SessionPayload = {
    email: string;
    given_name: string;
    family_name: string;
    roles?: Role[];
};
