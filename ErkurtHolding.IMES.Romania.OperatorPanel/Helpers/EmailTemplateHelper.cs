namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class EmailTemplateHelper
    {
        public static string FaultCauseMail()
        {
            return @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <title>{Type} Bildirimi</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                padding: 20px;
            }
            .container {
                max-width: 600px;
                background-color: #ffffff;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                margin: auto;
                text-align: left;
            }
            h2 {
                text-align: center;
                color: #333;
                font-size: 22px;
            }
            p {
                font-size: 16px;
                color: #555;
            }
            .highlight {
                font-weight: bold;
                color: #d32f2f;
            }
            .bold {
                font-weight: bold;
            }
            .details {
                margin-top: 15px;
            }
            .details ul {
                list-style-type: none;
                padding: 0;
            }
            .details li {
                margin: 5px 0;
                font-size: 16px;
            }
            .footer {
                text-align: center;
                font-size: 12px;
                color: #777;
                margin-top: 20px;
                padding-top: 10px;
                border-top: 1px solid #ddd;
            }
        </style>
    </head>
    <body>
    <div class='container'>
        <h2>{Type} Bildirimi</h2>
        <p>Merhaba <span class='highlight'>{Yetkili}</span>,</p>
        <p><span class='highlight'>{CreatedBy}</span> tarafından <span class='highlight'>{WorkCenterName}</span> iş merkezinde bir {Type} bildirimi yapılmıştır.</p>

        <div class='details'>
            <p><strong>Detaylar:</strong></p>
            <ul>
                <li><span class='bold'>• {Type} Şube:</span> {BranchName}</li>
                <li><span class='bold'>• {Type} Ana Sebep:</span> {MainCause}</li>
                <li><span class='bold'>• {Type} Açıklaması:</span> {CauseDetail}</li>
                <li><span class='bold'>• İş Merkezi:</span> {WorkCenterName}</li>
                <li><span class='bold'>• Kaynak:</span> {ResourceName}</li>
                <li><span class='bold'>• {Type} Başlangıç Tarihi:</span> {Date}</li>
                <li><span class='bold'>• {Type} Toplam Geçen Süresi:</span> {ElapsedDuration} DAKİKA</li>
            </ul>
        </div>

        <p>Bilginize sunarız.</p>

        <p class='footer'>Bu e-posta otomatik olarak oluşturulmuştur, lütfen yanıt vermeyiniz.</p>
    </div>
    </body>
    </html>";
        }
    }
}
