CREATE TABLE IF NOT EXISTS public.users
(
    id uuid NOT NULL,
    email_address text NOT NULL,
    password text,
    password_reset_token uuid,
    first_name text NOT NULL,
    last_name text NOT NULL,
    date_of_birth date NOT NULL,
    is_locked_out boolean NOT NULL DEFAULT false,
    roles text[],
    search_vector tsvector GENERATED ALWAYS AS (to_tsvector('english', first_name || ' ' || last_name || ' ' || email_address)) STORED,
    CONSTRAINT pk_users PRIMARY KEY (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_users_email_address ON public.users (email_address);

CREATE INDEX IF NOT EXISTS idx_users_search_vector ON public.users USING gin (search_vector);