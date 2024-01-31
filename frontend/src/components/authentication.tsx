import { FormEvent, useState } from 'react'
import { LoginCredentials, LoginResponse, Registration } from '../model'

interface AuthenticationViewProps {
    authenticationSuccessful: (loginResponse: LoginResponse) => void
}

interface LoginProps {
    switchRegister: () => void,
    authenticationSuccessful: (loginResponse: LoginResponse) => void
}

interface RegisterProps {
    switchLogin: () => void,
    authenticationSuccessful: (loginResponse: LoginResponse) => void
}

const AuthenticationView: React.FC<AuthenticationViewProps> = ({ authenticationSuccessful }) => {
    const [showLogin, setShowLogin] = useState(true);

    return (
        <div className='h-full flex items-center'>
            {
                showLogin ? <Login switchRegister={() => setShowLogin(false)} authenticationSuccessful={authenticationSuccessful} /> :
                    <Register switchLogin={() => setShowLogin(true)} authenticationSuccessful={authenticationSuccessful} />
            }
        </div>
    )
}

const Login: React.FC<LoginProps> = ({ switchRegister, authenticationSuccessful }) => {
    const [loginError, setLoginError] = useState<string | undefined>()

    const onLoginFormSubmit = async (e: FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault()

        const formData = new FormData(e.currentTarget)

        const username = formData.get('username')!.toString()
        const password = formData.get('password')!.toString()
        const body: LoginCredentials = { username, password }

        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/login`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(body)
        })

        if (response.ok) {
            const loginResponse: LoginResponse = await response.json()

            if (loginResponse) {
                authenticationSuccessful(loginResponse)
            } else {
                setLoginError(() => 'Invalid Credentials')
            }
        } else {
            setLoginError(() => 'Invalid Credentials')
        }
    }

    return (
        <form className='p-10 border rounded-xl shadow w-2/5 mx-auto my-auto' onSubmit={e => onLoginFormSubmit(e)}>
            <h3 className='text-center text-4975bc'>Login to Starcoder</h3>
            <h1 className='text-center text-292f7b'>Demo Project</h1>
            <hr className='mb-16' />
            {
                loginError && <p className="p-2 mb-2 text-sm text-center text-red-500">
                    {loginError}
                </p>
            }
            <div className="flex items-center mb-4">
                <label htmlFor="username" className='w-1/6'>Username</label>
                <input type="text" name="username" id="username" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Username' />
            </div>
            <div className="flex items-center mb-4">
                <label htmlFor="password" className='w-1/6'>Password</label>
                <input type="password" name="password" id="password" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Password' />
            </div>
            <button type="submit" className='w-full bg-blue-500 p-2 rounded text-white mb-4'>Login</button><hr className='mb-4' />
            <p className='text-center'>
                Do not have an account?
                <br />
                <button className='text-sm bg-orange-400 bg-opacity-20 p-1 rounded hover:bg-opacity-40' onClick={() => switchRegister()}>Register</button>
            </p>
        </form>
    )
}

const Register: React.FC<RegisterProps> = ({ switchLogin, authenticationSuccessful }) => {
    const [registrationError, setRegistrationError] = useState<string | undefined>()

    const onRegistrationFormSubmit = async (e: FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault()

        const registrationData = new FormData(e.currentTarget)

        const body: Registration = {
            firstname: registrationData.get('firstname')!.toString(),
            lastname: registrationData.get('lastname')!.toString(),
            username: registrationData.get('username')!.toString(),
            email: registrationData.get('email')!.toString(),
            password: registrationData.get('password')!.toString(),
        }

        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/register`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(body)
        })

        if (response.ok) {
            const loginResponse: LoginResponse = await response.json()

            if (loginResponse) {
                authenticationSuccessful(loginResponse)
            } else {
                setRegistrationError(() => 'Input error')
            }
        } else {
            setRegistrationError(() => 'Input error')
        }
    }

    return (
        <form className='p-10 border rounded-xl shadow w-2/5 mx-auto my-auto' onSubmit={e => onRegistrationFormSubmit(e)}>
            <h3 className='text-center text-4975bc'>Register to Starcoder</h3>
            <h1 className='text-center text-292f7b'>Demo Project</h1>
            <hr className='mb-16' />
            {
                registrationError && <p className="p-2 mb-2 text-sm text-center text-red-500">
                    {registrationError}
                </p>
            }
            <div className="flex mb-4">
                <label className='w-3/12 p-2'>Name</label>
                <div className="flex gap-4 w-9/12">
                    <input type="text" name="firstname" id="lastname" required className='drop-shadow border rounded w-1/2 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='First Name' />
                    <input type="text" name="lastname" id="lastname" required className='drop-shadow border rounded w-1/2 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Last Name' />
                </div>
            </div>
            <div className="flex mb-4">
                <label htmlFor="username" className='w-3/12 p-2'>Username</label>
                <div className='w-9/12'>
                    <input type="text" name="username" id="username" required className='drop-shadow border rounded w-full p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Username' />
                </div>
            </div>
            <div className="flex mb-4">
                <label htmlFor="email" className='w-3/12 p-2'>Email</label>
                <div className='w-9/12'>
                    <input type="email" name="email" id="email" required className='drop-shadow border rounded w-full p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Username' />
                </div>
            </div>
            <div className="flex mb-4">
                <label htmlFor="password" className='w-3/12 p-2'>Password</label>
                <input type="password" name="password" id="password" required className='drop-shadow border rounded w-9/12 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Password' />
            </div>
            <button type="submit" className='w-full bg-blue-500 p-2 rounded text-white mb-4'>Sign Up</button>
            <hr className='mb-4' />
            <p className='text-center'>
                Alread have an account?
                <br />
                <button className='text-sm bg-orange-400 bg-opacity-20 p-1 rounded hover:bg-opacity-40' onClick={() => switchLogin()}>Sign in</button>
            </p>
        </form>
    )
}

export { AuthenticationView }
