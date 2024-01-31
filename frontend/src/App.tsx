import { useState } from 'react'
import { LoginResponse } from './model'
import logo from './assets/images/logo.png'
import mugshot from './assets/images/mugshot.png'
import { AuthenticationView } from './components/authentication'
import { Dashboard } from './components/dashboard'

function App() {
  const [user, setUser] = useState<LoginResponse | null>(null)
  const [showLogin, setShowLogin] = useState(false)
  const [showDropdown, setShowDropdown] = useState(false)

  const logout = () => {
    fetch(`${import.meta.env.VITE_API_URL}/api/auth/logout`)
      .then(() => setUser(null))
  }

  return (
    <div className='h-screen flex flex-col'>
      <nav className='h-20 flex justify-between items-center px-4 bg-bs-color'>
        <div className="h-12">
          <img src={logo} alt="BS Logo" className='h-full' />
        </div>
        {
          user ? <button onClick={() => setShowDropdown((v) => !v)} className={`h-12 items-center flex gap-2 py-2 px-4 rounded shadow ${showDropdown ? 'bg-blue-100 shadow-black' : 'bg-orange-300'} relative hover:shadow-black hover:bg-blue-100 hover:text-black active:shadow-none active:text-white active:bg-gray-500`}>
            <img src={mugshot} alt="" className='rounded-full h-full' />
            <p>Welcome back {user.firstname}</p>
            {
              showDropdown && <div className="absolute w-full top-[calc(100%+0.5rem)] right-0 ">
                <button className='p-3 w-full bg-orange-300 rounded' onClick={() => logout()}>Logout</button>
              </div>
            }
          </button> : <button onClick={() => setShowLogin((prev) => !prev)} className='py-2 px-4 rounded shadow bg-orange-300 hover:shadow-black hover:bg-blue-100'>{showLogin ? 'Close' : 'Join us'}</button>
        }
      </nav>
      <div className='flex-1 overflow-scroll'>
        {
          user ? <Dashboard user={user} /> : showLogin ? <AuthenticationView authenticationSuccessful={loginResponse => setUser(() => loginResponse)} /> : <h1 className="text-3xl font-bold underline">
            Please Login
          </h1>
        }
      </div>
    </div>
  )
}

export default App
