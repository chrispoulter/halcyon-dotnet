import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import { cn } from '@/lib/utils';

type LoadingButtonProps = { loading?: boolean } & React.ComponentProps<
    typeof Button
>;

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
