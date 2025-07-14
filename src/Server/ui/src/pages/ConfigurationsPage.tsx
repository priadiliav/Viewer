import { useEffect, useState } from "react";
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';

export interface Configuration {
id: string;
name: string;
policyIds: string[];
processIds: string[];
}

export default function ConfigurationsPage() {
    const [configurations, setConfigurations] = useState<Configuration[]>([]);

    useEffect(() => {
        fetch('https://localhost:7041/api/configurations')
        .then(response => response.json())
        .then(data => {
            setConfigurations(data);
        })
    }, []);

    return (
        <>
            <Box sx={{ display: 'flex', mb:2 }}>
                <Button variant="contained" color="primary" onClick={() => alert('Add Agent')}>
                    Add Configuration
                </Button>
            </Box> 
            <TableContainer component={Paper}>
                <Table aria-label="agents table">
                <TableHead>
                    <TableRow>
                    <TableCell>ID</TableCell>
                    <TableCell>Name</TableCell>
                    <TableCell>Count of processes</TableCell>
                    <TableCell>Count of policies</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {configurations.map((configuration) => (
                    <TableRow key={configuration.id}>
                        <TableCell>{configuration.id}</TableCell>
                        <TableCell>{configuration.name}</TableCell>
                        <TableCell>{configuration.processIds.length}</TableCell>
                        <TableCell>{configuration.policyIds.length}</TableCell>
                    </TableRow>))}
                </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}
