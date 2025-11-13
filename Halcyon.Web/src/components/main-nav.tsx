import { useState } from 'react';
import { Link } from 'react-router';
import { Menu } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
    Drawer,
    DrawerContent,
    DrawerDescription,
    DrawerHeader,
    DrawerTitle,
    DrawerTrigger,
} from '@/components/ui/drawer';
import { useAuth } from '@/components/auth-provider';
import { type Role, isUserAdministrator } from '@/lib/session';

type MainNavRoute = { href: string; label: string; roles?: Role[] };

const routes: MainNavRoute[] = [
    { href: '/', label: 'Home' },
    { href: '/about', label: 'About' },
    {
        href: '/users',
        label: 'Users',
        roles: isUserAdministrator,
    },
];

export function MainNav() {
    const [open, setOpen] = useState(false);

    const { user } = useAuth();

    const routeLinks = routes
        .filter(
            ({ roles }) =>
                !roles || roles.some((value) => user?.roles?.includes(value))
        )
        .map(({ href, label }) => (
            <Button key={href} asChild variant="link">
                <Link to={href} onClick={() => setOpen(false)}>
                    {label}
                </Link>
            </Button>
        ));

    return (
        <>
            <nav className="hidden gap-2 sm:flex">{routeLinks}</nav>
            <Drawer open={open} onOpenChange={setOpen}>
                <DrawerTrigger asChild>
                    <Button variant="outline" size="icon" className="sm:hidden">
                        <Menu />
                        <span className="sr-only">Toggle main menu</span>
                    </Button>
                </DrawerTrigger>
                <DrawerContent>
                    <div className="mx-auto w-full max-w-sm">
                        <DrawerHeader className="sr-only">
                            <DrawerTitle>Halcyon</DrawerTitle>
                            <DrawerDescription>Main Menu</DrawerDescription>
                        </DrawerHeader>
                        <nav className="flex flex-col items-stretch justify-center gap-2 p-4">
                            {routeLinks}
                        </nav>
                    </div>
                </DrawerContent>
            </Drawer>
        </>
    );
}
