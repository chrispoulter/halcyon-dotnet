import ky from 'ky';

type ProblemDetails = {
    status: number;
    title: string;
    traceId: string;
    type: string;
};

export const apiClient = ky.create({
    hooks: {
        beforeError: [
            async (error) => {
                const contentType = error.response.headers.get('content-type');

                if (contentType?.includes('application/problem+json')) {
                    const body = await error.response.json<ProblemDetails>();
                    error.message = body.title;
                }

                return error;
            },
        ],
    },
});
