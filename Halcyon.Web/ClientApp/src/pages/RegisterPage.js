import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Helmet } from 'react-helmet';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import { Container, FormGroup } from 'reactstrap';
import {
    TextInput,
    DateInput,
    Button,
    AuthContext,
    useFetch
} from '../components';
import { trackEvent } from '../utils/logger';

export const RegisterPage = ({ history }) => {
    const { setToken } = useContext(AuthContext);

    const { refetch: register } = useFetch({
        method: 'POST',
        url: '/account/register',
        manual: true
    });

    const { refetch: generateToken } = useFetch({
        method: 'POST',
        url: '/token',
        manual: true
    });

    const onSubmit = async variables => {
        let result = await register({
            emailAddress: variables.emailAddress,
            password: variables.password,
            firstName: variables.firstName,
            lastName: variables.lastName,
            dateOfBirth: variables.dateOfBirth
        });

        if (result.ok) {
            trackEvent('sign_up', {
                entityId: result.data.id
            });

            result = await generateToken({
                grantType: 'PASSWORD',
                emailAddress: variables.emailAddress,
                password: variables.password
            });

            if (result.ok) {
                setToken(result.data.accessToken);
                history.push('/');
            }
        }
    };

    return (
        <Container>
            <Helmet>
                <title>Register</title>
            </Helmet>

            <h1>Register</h1>
            <hr />

            <Formik
                initialValues={{
                    emailAddress: '',
                    password: '',
                    confirmPassword: '',
                    firstName: '',
                    lastName: '',
                    dateOfBirth: ''
                }}
                validationSchema={Yup.object().shape({
                    emailAddress: Yup.string()
                        .label('Email Address')
                        .max(254)
                        .email()
                        .required(),
                    password: Yup.string()
                        .label('Password')
                        .min(8)
                        .max(50)
                        .required(),
                    confirmPassword: Yup.string()
                        .label('Confirm Password')
                        .required()
                        .oneOf([Yup.ref('password')]),
                    firstName: Yup.string()
                        .label('First Name')
                        .max(50)
                        .required(),
                    lastName: Yup.string()
                        .label('Last Name')
                        .max(50)
                        .required(),
                    dateOfBirth: Yup.string().label('Date Of Birth').required()
                })}
                onSubmit={onSubmit}
            >
                {({ isSubmitting }) => (
                    <Form noValidate>
                        <Field
                            name="emailAddress"
                            type="email"
                            label="Email Address"
                            required
                            maxLength={254}
                            autoComplete="username"
                            component={TextInput}
                        />

                        <Field
                            name="password"
                            type="password"
                            label="Password"
                            required
                            maxLength={50}
                            autoComplete="new-password"
                            component={TextInput}
                        />

                        <Field
                            name="confirmPassword"
                            type="password"
                            label="Confirm Password"
                            required
                            maxLength={50}
                            autoComplete="new-password"
                            component={TextInput}
                        />

                        <Field
                            name="firstName"
                            type="text"
                            label="First Name"
                            required
                            maxLength={50}
                            component={TextInput}
                        />

                        <Field
                            name="lastName"
                            type="text"
                            label="Last Name"
                            required
                            maxLength={50}
                            component={TextInput}
                        />

                        <Field
                            name="dateOfBirth"
                            type="date"
                            label="Date Of Birth"
                            required
                            component={DateInput}
                        />

                        <FormGroup className="text-right">
                            <Button
                                type="submit"
                                color="primary"
                                loading={isSubmitting}
                            >
                                Submit
                            </Button>
                        </FormGroup>
                    </Form>
                )}
            </Formik>

            <p>
                Already have an account? <Link to="/login">Log in now</Link>
            </p>
        </Container>
    );
};
