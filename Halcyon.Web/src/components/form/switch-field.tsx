import {
    Controller,
    type Control,
    type FieldPath,
    type FieldValues,
} from 'react-hook-form';
import {
    Field,
    FieldContent,
    FieldDescription,
    FieldError,
    FieldLabel,
} from '@/components/ui/field';
import { Switch } from '@/components/ui/switch';

interface SwitchFieldProps<T extends FieldValues> {
    control: Control<T>;
    name: FieldPath<T>;
    disabled?: boolean;
    options: Record<string, { title: string; description: string }>;
}

export function SwitchField<T extends FieldValues>({
    control,
    name,
    disabled,
    options,
}: SwitchFieldProps<T>) {
    return (
        <Controller
            name={name}
            control={control}
            render={({ field, fieldState }) => {
                const value: string[] = field.value || [];

                return (
                    <div className="space-y-2">
                        {Object.entries(options).map(
                            ([key, { title, description }]) => {
                                const checked = value?.includes(key);

                                function onCheckChanged(checked: boolean) {
                                    if (checked) {
                                        return field.onChange([...value, key]);
                                    }

                                    return field.onChange(
                                        value.filter((item) => item !== key)
                                    );
                                }

                                return (
                                    <Field
                                        key={key}
                                        orientation="horizontal"
                                        data-invalid={fieldState.invalid}
                                        className="rounded-md border p-4"
                                    >
                                        <FieldContent>
                                            <FieldLabel
                                                htmlFor={`${name}-${key}`}
                                            >
                                                {title}
                                            </FieldLabel>
                                            <FieldDescription>
                                                {description}
                                            </FieldDescription>
                                            {fieldState.invalid && (
                                                <FieldError
                                                    errors={[fieldState.error]}
                                                />
                                            )}
                                        </FieldContent>
                                        <Switch
                                            id={`${name}-${key}`}
                                            name={name}
                                            checked={checked}
                                            value={key}
                                            onCheckedChange={onCheckChanged}
                                            disabled={disabled}
                                            aria-invalid={fieldState.invalid}
                                        />
                                    </Field>
                                );
                            }
                        )}
                    </div>
                );
            }}
        />
    );
}
