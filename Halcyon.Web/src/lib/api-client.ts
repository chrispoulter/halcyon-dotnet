import ky, { HTTPError } from 'ky';
import { config } from './config';

interface ProblemDetails {
    title: string;
}

export const apiClient = ky.create({
    prefix: config.VITE_API_URL,
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
                    const body = error.data as ProblemDetails;
                    error.message = body?.title || error.message;
                }
                return error;
            },
        ],
    },
});
