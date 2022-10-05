# LocalRecon
C# tool to perform local reconnaissance and check for potential privilege escalation vectors.

This tool will perform many of the same checks as [WinPeas](https://github.com/carlospolop/PEASS-ng), [Seatbelt](https://github.com/GhostPack/Seatbelt), and various other Windows recon/priv esc checking tools.  This tool is mainly a way for me to practice performing these activities in C# and potentially have a tool to do so that is more likely to bypass AV detections than the well known tools mentioned before.

## Current options
- Run system command
- Local user (and administrators)
- Local listening ports
- List all local services
- Check for unquoted service paths

## Example Running (So far)
![image](https://user-images.githubusercontent.com/58894272/186997857-e67e22fa-bc08-4c96-8917-c67519e98c1d.png)


# To-Do
- [ ] Add command line argument parsing
