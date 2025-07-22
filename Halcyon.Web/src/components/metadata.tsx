type MetadataProps = {
    title: string;
};

export function Metadata({ title }: MetadataProps) {
    return <title>{`${title} // Halcyon`}</title>;
}
