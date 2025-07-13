import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { createTheme, ThemeProvider } from '@mui/material/styles';

import Layout from './components/layout/Layout';
import AgentsPage from './pages/AgentsPage';
import ConfigurationsPage from './pages/ConfigurationsPage';
import ProcessPage from './pages/ProcessPage';
import PolicyPage from './pages/PolicyPage';
import DashboardPage from './pages/DashboardPage';

const demoTheme = createTheme({
  cssVariables: {
    colorSchemeSelector: 'data-toolpad-color-scheme',
  },
  colorSchemes: { light: true, dark: true },
  breakpoints: {
    values: {
      xs: 0,
      sm: 600,
      md: 600,
      lg: 1200,
      xl: 1536,
    },
  },
});


export default function App() {
  return (
    <ThemeProvider theme={demoTheme}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="dashboard" element={<DashboardPage />} />
            <Route path="agents" element={<AgentsPage />} />
            <Route path="configurations" element={<ConfigurationsPage />} />
            <Route path="processes" element={<ProcessPage />} />
            <Route path="policies" element={<PolicyPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}