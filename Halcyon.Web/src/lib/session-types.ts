export type Role = 'SYSTEM_ADMINISTRATOR' | 'USER_ADMINISTRATOR';

export const roles: Record<Role, { title: string; description: string }> = {
    SYSTEM_ADMINISTRATOR: {
        title: 'System Administrator',
        description: 'A system administrator has access to the entire system.',
    },
    USER_ADMINISTRATOR: {
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
