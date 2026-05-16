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
import { isUserAdministrator } from '@/lib/session';
import { useAuth } from './auth-provider';
import { ModeToggle } from './mode-toggle';
import { UserMenu } from './user-menu';

const navItems = [
    { href: '/', label: 'Home' },
    { href: '/about', label: 'About' },
    {
        href: '/users',
        label: 'Users',
        roles: isUserAdministrator,
    },
];

export function Header() {
    const [open, setOpen] = useState(false);

    const { user } = useAuth();

    const navLinks = navItems
        .filter(
            ({ roles }) =>
                !roles || roles.some((value) => user?.roles?.includes(value))
        )
        .map(({ href, label }) => (
            <Button key={href} asChild variant="ghost">
                <Link to={href} onClick={() => setOpen(false)}>
                    {label}
                </Link>
            </Button>
        ));

    return (
        <header className="mb-6 border-b">
            <div className="mx-auto flex max-w-screen-sm items-center gap-2 px-6 py-4 sm:px-0">
                <div className="flex items-center gap-2">
                    <Link
                        to="/"
                        className="scroll-m-20 text-xl font-semibold tracking-tight"
                    >
                        Halcyon
                    </Link>
                </div>

                <div className="ml-auto flex items-center gap-2">
                    <nav className="hidden gap-2 sm:flex">{navLinks}</nav>
                    <Drawer open={open} onOpenChange={setOpen}>
                        <DrawerTrigger asChild>
                            <Button
                                variant="outline"
                                size="icon"
                                className="sm:hidden"
                            >
                                <Menu />
                                <span className="sr-only">
                                    Toggle main menu
                                </span>
                            </Button>
                        </DrawerTrigger>
                        <DrawerContent>
                            <div className="mx-auto w-full max-w-sm">
                                <DrawerHeader className="sr-only">
                                    <DrawerTitle>Halcyon</DrawerTitle>
                                    <DrawerDescription>
                                        Main Menu
                                    </DrawerDescription>
                                </DrawerHeader>
                                <nav className="flex flex-col items-stretch justify-center gap-2 p-4">
                                    {navLinks}
                                </nav>
                            </div>
                        </DrawerContent>
                    </Drawer>
                    <ModeToggle />
                    <UserMenu />
                </div>
            </div>
        </header>
    );
}
