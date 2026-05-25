import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import { cn } from '@/lib/utils';

interface LoadingButtonProps extends React.ComponentProps<typeof Button> {
    loading?: boolean;
}

export function LoadingButton({
    loading,
    children,
    disabled,
    ...rest
}: LoadingButtonProps) {
    return (
        <Button {...rest} disabled={disabled || loading}>
            {loading && <Spinner className="absolute" />}
            <span
                className={cn({
                    invisible: loading,
                })}
            >
                {children}
            </span>
        </Button>
    );
}
