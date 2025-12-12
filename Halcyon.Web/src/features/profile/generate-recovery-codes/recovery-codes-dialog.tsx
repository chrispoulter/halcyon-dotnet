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
    async function onCopyToClipboard() {
        const text = codes?.join('\n') || '';
        await navigator.clipboard.writeText(text);
        toast.success('Recovery codes copied to clipboard.');
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

                <div className="bg-muted rounded px-3 py-1 font-mono font-semibold">
                    {codes?.map((code) => (
                        <>
                            {code}
                            <br />
                        </>
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
