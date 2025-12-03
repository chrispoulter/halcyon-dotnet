import ky from 'ky';

type ProblemDetails = {
    status: number;
    title: string;
    traceId: string;
    type: string;
};

export const apiClient = ky.create({
    prefixUrl: '/api',
    hooks: {
        beforeRequest: [
            (request, options) => {
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
            async (error) => {
                const { response } = error;
                const contentType = response.headers.get('content-type');

                if (contentType?.includes('application/problem+json')) {
                    const body = await response.json<ProblemDetails>();
                    error.message = body.title;
                }

                return error;
            },
        ],
    },
});
