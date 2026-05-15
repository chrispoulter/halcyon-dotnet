import { HTTPError } from 'ky';
import { ErrorPage } from '@/error-page';
import { ForbiddenPage } from '@/forbidden-page';
import { NotFoundPage } from '@/not-found-page';
import { LogoutRedirect } from './logout-redirect';

type QueryErrorProps = { error: Error | null };

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
