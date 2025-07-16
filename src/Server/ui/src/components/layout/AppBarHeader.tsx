import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

interface AppBarHeaderProps {
  drawerWidth: number;
}

export default function AppBarHeader(props: AppBarHeaderProps) {
  const apiUrl = import.meta.env.VITE_API_URL;
  if (!apiUrl) {
      console.error('VITE_API_URL is not defined');
      return null;
  }
  console.log('VITE_API_URL:', apiUrl);
  
  return (
    <AppBar
      position="fixed"
      sx={{ width: `calc(100% - ${props.drawerWidth}px)`, ml: `${props.drawerWidth}px` }}
    >
      <Toolbar>
        test
      </Toolbar>
    </AppBar>
  );
}
