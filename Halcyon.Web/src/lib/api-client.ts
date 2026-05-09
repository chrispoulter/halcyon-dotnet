import ky, { HTTPError } from 'ky';

export const apiClient = ky.create({
    prefix: '/api',
    hooks: {
        beforeRequest: [
            ({ request, options }) => {
                const { accessToken } = options.context;

                if (accessToken) {
                    request.headers.set(
                        'Authorization',
                        `Bearer ${accessToken}`
                    );
                }
            },
        ],
        beforeError: [
            async ({ error }) => {
                if (error instanceof HTTPError) {
                    const { response } = error;
                    const contentType = response.headers.get('content-type');

                    if (contentType?.includes('application/problem+json')) {
                        const body = await response.json();
                        error.message = body.title;
                    }
                }

                return error;
            },
        ],
    },
});
