import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import { useEffect, useState } from 'react';


interface Policy {
    id: string;
    name: string;
    description: string;
    registryPath: string;
    registryKeyType: string;
    registryKey: string;
    registryValueType: string;
    registryValue: string;
}


export default function PolicyPage() {
    const [policies, setPolicies] = useState<Policy[]>([]);

    useEffect(() => {
        fetch('https://localhost:7041/api/policies')
            .then(response => response.json())
            .then(data => {
                setPolicies(data);
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
                    {policies.map((policy) => (
                        <TableRow key={policy.id}>
                        <TableCell>{policy.id}</TableCell>
                        <TableCell>{policy.name}</TableCell>
                        <TableCell>{policy.description}</TableCell>
                        <TableCell>{policy.registryPath}</TableCell>
                        <TableCell>{policy.registryKeyType}</TableCell>
                        <TableCell>{policy.registryKey}</TableCell>
                        <TableCell>{policy.registryValueType}</TableCell>
                        <TableCell>{policy.registryValue}</TableCell>
                    </TableRow>))}
                </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}
