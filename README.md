# LocalRecon
C# tool to perform local reconnaissance and check for potential privilege escalation vectors.

This tool will perform many of the same checks as [WinPeas](https://github.com/carlospolop/PEASS-ng), [Seatbelt](https://github.com/GhostPack/Seatbelt), and various other Windows recon/priv esc checking tools.  This tool is mainly a way for me to practice performing these activities in C# and potentially have a tool to do so that is more likely to bypass AV detections than the well known tools mentioned before.

It runs all current checks by default for now, but I will eventually add command-line parsing to specify what should be checked.

## Current Checks
- Current user's groups
- Local user (and administrators)
- List listening ports 
- Check for unquoted service paths and services our user can restart
- List mapped network drives

## Example Running (So far)
<img width="695" alt="image" src="https://user-images.githubusercontent.com/58894272/221679626-c9935544-04fe-476f-aad0-724184759ed3.png">


# To-Do
- [ ] Add command line argument parsing
