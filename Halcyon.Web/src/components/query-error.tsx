import { HTTPError } from 'ky';
import { ErrorPage } from '@/pages/error-page';
import { ForbiddenPage } from '@/pages/forbidden-page';
import { NotFoundPage } from '@/pages/not-found-page';
import { LogoutRedirect } from './logout-redirect';

interface QueryErrorProps {
    error: Error | null;
}

export function QueryError({ error }: QueryErrorProps) {
    if (error instanceof HTTPError) {
        switch (error.response.status) {
            case 401:
                return <LogoutRedirect />;

            case 403:
                return <ForbiddenPage />;

            case 404:
                return <NotFoundPage />;
        }
    }

    return <ErrorPage />;
}
