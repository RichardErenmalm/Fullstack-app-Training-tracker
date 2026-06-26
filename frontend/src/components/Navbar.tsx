import { NavLink } from 'react-router-dom';

const Navbar: React.FC = () => {
  return (
    <nav className="navbar">
      <div className="navbar-inner">
        <span className="navbar-brand">Training Tracker</span>
        <div className="navbar-links">
          <NavLink to="/" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} end>
            Mina Workouts
          </NavLink>
          <NavLink to="/exercises" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
            Mina Övningar
          </NavLink>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
