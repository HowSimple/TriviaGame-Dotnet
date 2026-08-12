import { useAuth0Context } from "../auth/Auth0";

const LoginButton = () => {
  const { login } = useAuth0Context();
  const handleLogin = () => {
  sessionStorage.setItem('returnTo', window.location.pathname)
  login()
}
  return (
    <button 
      onClick={() => handleLogin()} 
      className="button login"
    >
      Log In
    </button>
  );
};

export default LoginButton;