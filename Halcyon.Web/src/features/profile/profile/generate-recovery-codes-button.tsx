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
    className?: string;
};

export function GenerateRecoveryCodesButton({
    className,
}: GenerateRecoveryCodesButtonProps) {
    const [recoveryCodes, setRecoveryCodes] = useState<string[] | undefined>();
    const [showDialog, setShowDialog] = useState(false);

    const { mutate: generateRecoveryCodes, isPending: isGenerating } =
        useGenerateRecoveryCodes();

    function onGenerate() {
        generateRecoveryCodes(undefined, {
            onSuccess: (data) => {
                toast.success('Recovery codes have been generated.');
                setRecoveryCodes(data.recoveryCodes);
                setShowDialog(true);
            },
            onError: (error) => toast.error(error.message),
        });
    }

    return (
        <>
            <AlertDialog>
                <AlertDialogTrigger asChild>
                    <LoadingButton loading={isGenerating} className={className}>
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
                            disabled={isGenerating}
                            onClick={onGenerate}
                        >
                            Continue
                        </AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>

            <RecoveryCodesDialog
                open={showDialog}
                onOpenChange={setShowDialog}
                codes={recoveryCodes}
            />
        </>
    );
}
