import Table from '@mui/material/Table';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import { useEffect, useState } from 'react';

interface Process {
    id: string;
    name: string;
    status: string;
}


export default function ProcessPage() {
    const [processes, setProcesses] = useState<Process[]>([]);
    
    useEffect(() => {
        fetch('https://localhost:7041/api/processes')
            .then(response => response.json())
            .then(data => {
                setProcesses(data);
            })
    }, []);

    return (
        <>
            <Box sx={{ display: 'flex', mb: 2 }}>
                <Button variant="contained" color="primary" onClick={() => alert('Add Process')}>
                    Add Process
                </Button>
            </Box>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Process Name</TableCell>
                            <TableCell>Status</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {processes.map((process) => (
                            <TableRow key={process.id}>
                                <TableCell>{process.name}</TableCell>
                                <TableCell>{process.status}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}
