///made by W3bParser on GitHub

private void btnScan_Click(object sender, EventArgs e)
{
    string subnet = txtSubnet.Text;
    progressBar.Maximum = 254;
    progressBar.Value = 0;
    lvResult.Items.Clear();
 
    Task.Factory.StartNew(new Action(() =>
    {
        for (int i = 2; i < 255; i++)
        {
            string ip = $"{subnet}.{i}";
            Ping ping = new Ping();
            PingReply reply = ping.Send(ip, 100);
            if (reply.Status == IPStatus.Success)
            {
                progressBar.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        IPHostEntry host = Dns.GetHostEntry(IPAddress.Parse(ip));
                        lvResult.Items.Add(new ListViewItem(new String[] { ip, host.HostName, "Up" }));
                    }
                    catch
                    {
                        MessageBox.Show($"Couldn't retrieve hostname from {ip}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    progressBar.Value += 1;
                    lblStatus.ForeColor = Color.Blue;
                    lblStatus.Text = $"Scanning: {ip}";
                    if (progressBar.Value == 253)
                        lblStatus.Text = "Finished";
                }));
            }
            else
            {
                progressBar.BeginInvoke(new Action(() =>
                {
                    progressBar.Value += 1;
                    lblStatus.ForeColor = Color.DarkGray;
                    lblStatus.Text = $"Scanning: {ip}";
                    lvResult.Items.Add(new ListViewItem(new String[] { ip, "", "Down" }));
                    if (progressBar.Value == 253)
                        lblStatus.Text = "Finished";
                }));
            }
        }
    }));
}
