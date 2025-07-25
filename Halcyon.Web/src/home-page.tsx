export function HomePage() {
    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Welcome!
            </h1>

            <p className="text-muted-foreground text-xl">
                Welcome to Halcyon, a modern React web project template built
                with a sense of peace and tranquillity. This template provides a
                solid foundation for building scalable web applications with the
                latest technologies and best practices.
            </p>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Built with Modern Tools
            </h2>

            <p className="text-muted-foreground text-sm">
                Halcyon leverages cutting-edge technologies including React
                Router, TypeScript for type safety, React Query for server state
                management, and Tailwind CSS with shadcn/ui components for
                beautiful, accessible user interfaces. The template includes
                authentication, form handling with React Hook Form and Zod
                validation, and a comprehensive development setup with ESLint
                and Prettier.
            </p>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Ready for Production
            </h2>

            <p className="text-muted-foreground text-sm">
                This template comes with Docker configuration, comprehensive
                routing, user management features, and a modern build pipeline.
                Whether you&apos;re building a small application or a
                large-scale project, Halcyon provides the structure and tools
                you need to get started quickly and maintain code quality as you
                grow.
            </p>
        </main>
    );
}
