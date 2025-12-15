import { useState } from 'react';
import { toast } from 'sonner';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { LoadingButton } from '@/components/loading-button';
import { useGenerateRecoveryCodes } from '@/features/profile/hooks/use-generate-recovery-codes';
import { RecoveryCodesDialog } from '@/features/profile/generate-recovery-codes/recovery-codes-dialog';

type GenerateRecoveryCodesButtonProps = {
    disabled?: boolean;
    className?: string;
};

export function GenerateRecoveryCodesButton({
    disabled,
    className,
}: GenerateRecoveryCodesButtonProps) {
    const [recoveryCodes, setRecoveryCodes] = useState<string[] | undefined>();

    const { mutate: generateRecoveryCodes, isPending: isGenerating } =
        useGenerateRecoveryCodes();

    function onGenerate() {
        generateRecoveryCodes(undefined, {
            onSuccess: (data) => {
                toast.success('Recovery codes have been generated.');
                setRecoveryCodes(data.recoveryCodes);
            },
            onError: (error) => toast.error(error.message),
        });
    }

    function onOpenChange(open: boolean) {
        if (!open) {
            setRecoveryCodes(undefined);
        }
    }

    return (
        <>
            <AlertDialog>
                <AlertDialogTrigger asChild>
                    <LoadingButton
                        loading={isGenerating}
                        disabled={disabled}
                        className={className}
                    >
                        Generate Recovery Codes
                    </LoadingButton>
                </AlertDialogTrigger>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>
                            Generate Recovery Codes
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            Are you sure you want to generate new recovery
                            codes?
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                        <AlertDialogAction
                            disabled={disabled || isGenerating}
                            onClick={onGenerate}
                        >
                            Continue
                        </AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>

            <RecoveryCodesDialog
                open={!!recoveryCodes}
                onOpenChange={onOpenChange}
                codes={recoveryCodes}
            />
        </>
    );
}
