import React, { useState } from "react";
import { Box, Stack, Typography, TextField, Button, IconButton, InputAdornment, Alert, Snackbar } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useUrlShortener } from "../../../hooks/useUrlShortener";

function Redirector() {
    const [shortCode, setShortCode] = useState("");
    const [openUrl, setOpenUrl] = useState(false);
    const [openSCode, setOpenSCode] = useState(false);
    const [data, setData] = useState([]);
    const [orgUrl, setOrgUrl] = useState("");
    const [req, setReq] = useState("");
    const [isShorten, setIsShorten] = useState(false);
    const [openUpdate, setOpenUpdate] = useState(false);
    const [updatedUrl, setUpdatedUrl] = useState("");
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });

    const { shortenUrl, getUrlStats, updateUrl, deleteUrl, loading, error } = useUrlShortener();

    const handleSnackbarClose = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    const handleUrlSubmit = async () => {
        if (!orgUrl) {
            console.error("URL is required.");
            return;
        }
        if (!isShorten) {
            const result = await shortenUrl(orgUrl);
            if (result) {
                setReq(result.shortUrl);
                setOpenUrl(true);
                setIsShorten(true);
            }
        } else {
            setIsShorten(false);
            setOrgUrl("");
            setOpenUrl(false);
        }
    };

    const handleCopy = () => {
        navigator.clipboard.writeText(req);
        setSnackbar({
            open: true,
            message: "Shortened URL copied to clipboard!",
            severity: "success"
        });
    };

    const handleCopyUrl = () => {
        navigator.clipboard.writeText(data.url);
        setSnackbar({
            open: true,
            message: "Original URL copied to clipboard!",
            severity: "success"
        });
    };

    const handleUpdateSubmit = async () => {
        if (!shortCode || !updatedUrl) {
            console.error("Short code and URL are required.");
            return;
        }
        const result = await updateUrl(shortCode.split("/").pop(), updatedUrl);
        if (result) {
            setSnackbar({
                open: true,
                message: "URL successfully updated!",
                severity: "success"
            });
            setOpenUpdate(false);
            setUpdatedUrl("");
        }
    };

    const handleClear = () => {
        setOpenSCode(false);
        setOpenUpdate(false);
        setShortCode("");
    };

    const handleStaticsSubmit = async () => {
        setOpenUpdate(false);
        if (!shortCode) {
            console.error("Short code is required.");
            return;
        }
        const result = await getUrlStats(shortCode.split("/").pop());
        if (result) {
            setData(result);
            setOpenSCode(true);
        }
    };

    const handleDeleteSubmit = async () => {
        if (!shortCode) {
            console.error("Short code is required to delete.");
            return;
        }
        const success = await deleteUrl(shortCode.split("/").pop());
        if (success) {
            setSnackbar({
                open: true,
                message: "URL successfully deleted!",
                severity: "success"
            });
            setShortCode("");
            setOpenSCode(false);
        }
    };

    return (
        <>
            <Snackbar
                open={snackbar.open}
                autoHideDuration={2000}
                onClose={handleSnackbarClose}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert onClose={handleSnackbarClose} severity={snackbar.severity} sx={{ width: '100%' }}>
                    {snackbar.message}
                </Alert>
            </Snackbar>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            <Stack sx={{ width: "100%" }}>
                <Typography variant="h4" align="center" gutterBottom>
                    URL SHORTENER
                </Typography>

                <Box sx={{
                    width: { xs: "90%", sm: "70%", md: "50%" },
                    height: { xs: "auto", md: "30vh" },
                    mx: "auto",
                    my: { xs: 1, md: 3 },
                    boxShadow: 3,
                    borderRadius: 2,
                    bgcolor: "white",
                    p: { xs: 2, md: 3 },
                }}>
                    <Typography variant="h6" align="center" gutterBottom>
                        Enter a URL to shorten
                    </Typography>
                    <TextField
                        label="paste your URL here..."
                        variant="outlined"
                        fullWidth
                        value={orgUrl}
                        onChange={(e) => setOrgUrl(e.target.value)}
                        disabled={loading}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <Button
                                        size="small"
                                        variant="contained"
                                        onClick={handleUrlSubmit}
                                        disabled={loading}
                                    >
                                        {isShorten ? "Shorten again" : "Shorten"}
                                    </Button>
                                </InputAdornment>
                            ),
                        }}
                    />
                    {openUrl && (
                        <Box sx={{ my: 3 }} display="flex" alignItems="center" gap={3}>
                            <Typography>your shortened URL: {req}</Typography>
                            <Button size="small" variant="contained" onClick={handleCopy}>
                                Copy
                            </Button>
                        </Box>
                    )}
                </Box>

                <Box sx={{
                    width: { xs: "90%", sm: "70%", md: "50%" },
                    height: { xs: "60vh", md: "40vh" },
                    mx: "auto",
                    my: { xs: 1, md: 3 },
                    boxShadow: 3,
                    borderRadius: 2,
                    bgcolor: "white",
                    p: { xs: 2, md: 3 },
                }}>
                    <Typography gutterBottom variant="h6" align="center">
                        Retrieve Original URL
                    </Typography>
                    <TextField
                        label="Enter short code"
                        variant="outlined"
                        fullWidth
                        value={shortCode}
                        onChange={(e) => setShortCode(e.target.value)}
                        disabled={loading}
                        InputProps={{
                            endAdornment: (
                                shortCode && (
                                    <InputAdornment position="end">
                                        <IconButton onClick={handleClear} edge="end">
                                            <CloseIcon />
                                        </IconButton>
                                    </InputAdornment>
                                )
                            ),
                        }}
                    />

                    {openSCode && (
                        <Box sx={{ my: 3 }} display="flex" alignItems="center" gap={3}>
                            <Typography>
                                The original URL is: {data.url} with {data.accessCount} view
                            </Typography>
                            <Button size="small" variant="contained" onClick={handleCopyUrl}>
                                Copy URL
                            </Button>
                        </Box>
                    )}

                    {openUpdate && (
                        <Box sx={{ my: 3 }} display="flex" alignItems="center" gap={3}>
                            <TextField
                                label="Enter new url"
                                variant="outlined"
                                fullWidth
                                value={updatedUrl}
                                onChange={(e) => setUpdatedUrl(e.target.value)}
                                disabled={loading}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <Button
                                                size="small"
                                                variant="contained"
                                                onClick={handleUpdateSubmit}
                                                disabled={loading}
                                            >
                                                Save
                                            </Button>
                                        </InputAdornment>
                                    ),
                                }}
                            />
                        </Box>
                    )}

                    <Stack
                        direction={{ xs: "column", sm: "row" }}
                        spacing={2}
                        sx={{ my: 2 }}
                    >
                        <Button
                            variant="contained"
                            onClick={handleDeleteSubmit}
                            sx={{ flex: 1 }}
                            disabled={loading}
                        >
                            Delete
                        </Button>
                        <Button
                            variant="contained"
                            onClick={handleStaticsSubmit}
                            sx={{ flex: 1 }}
                            disabled={loading}
                        >
                            URL Stats
                        </Button>
                        <Button
                            variant="contained"
                            onClick={() => {
                                setOpenUpdate(true);
                                setOpenSCode(false);
                            }}
                            sx={{ flex: 1 }}
                            disabled={loading}
                        >
                            Update URL
                        </Button>
                    </Stack>
                </Box>
            </Stack>
        </>
    );
}

export default Redirector;