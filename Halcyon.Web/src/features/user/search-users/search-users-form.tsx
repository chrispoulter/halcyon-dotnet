import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Search } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { TextField } from '@/components/form/text-field';

const schema = z.object({
    search: z
        .string({
            message: 'Search must be a valid string',
        })
        .optional(),
});

export type SearchUsersFormValues = z.infer<typeof schema>;

type SearchUsersFormProps = {
    search?: string;
    onSubmit: (data: SearchUsersFormValues) => void;
    disabled?: boolean;
};

export function SearchUsersForm({
    search = '',
    onSubmit,
    disabled,
}: SearchUsersFormProps) {
    const form = useForm<SearchUsersFormValues>({
        resolver: zodResolver(schema),
        defaultValues: {
            search,
        },
    });

    return (
        <form
            noValidate
            onSubmit={form.handleSubmit(onSubmit)}
            className="flex w-full gap-2"
        >
            <TextField
                control={form.control}
                name="search"
                type="search"
                placeholder="Search Users..."
                disabled={disabled}
            />

            <Button
                type="submit"
                variant="secondary"
                size="icon"
                disabled={disabled}
            >
                <Search />
                <span className="sr-only">Search users</span>
            </Button>
        </form>
    );
}
