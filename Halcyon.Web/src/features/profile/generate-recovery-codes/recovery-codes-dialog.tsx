import { toast } from 'sonner';
import { ClipboardCopy } from 'lucide-react';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';

type RecoveryCodesDialogProps = {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    codes?: string[];
};

export function RecoveryCodesDialog({
    open,
    onOpenChange,
    codes,
}: RecoveryCodesDialogProps) {
    function onCopyToClipboard() {
        const text = codes?.join('\n') || '';

        navigator.clipboard
            .writeText(text)
            .then(() => toast.success('Recovery codes copied to clipboard.'))
            .catch(() =>
                toast.error('Failed to copy recovery codes to clipboard.')
            );
    }

    return (
        <AlertDialog open={open} onOpenChange={onOpenChange}>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Recovery Codes</AlertDialogTitle>
                    <AlertDialogDescription>
                        Put these codes in a safe place. If you lose your device
                        and don&apos;t have the recovery codes you will lose
                        access to your account.
                    </AlertDialogDescription>
                </AlertDialogHeader>

                <div className="bg-muted flex flex-col rounded px-2 py-1 font-mono font-semibold">
                    {codes?.map((code) => (
                        <span key={code}>{code}</span>
                    ))}
                </div>

                <AlertDialogFooter>
                    <Button variant="outline" onClick={onCopyToClipboard}>
                        <ClipboardCopy /> Copy
                    </Button>
                    <AlertDialogAction onClick={() => onOpenChange(false)}>
                        Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
