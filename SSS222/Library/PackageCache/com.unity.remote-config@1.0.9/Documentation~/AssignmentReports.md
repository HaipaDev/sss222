# Assignment reports
Assignment summary reports provide insight into how many unique users experienced each Rule. Use these reports to draw correlations between Rules and user behavior. The report is exported as a CSV file, consisting of ten columns:

| **Column** | **Description** |
| ---------- | --------------- |
| `projectId` | The application’s Unity Project ID, which you can locate on the [developer dashboard](https://operate.dashboard.unity3d.com/) (from the Project view, **Settings** > **Project Settings**). |
| `environmentId` | A unique identifier for the environment served for an assignment. |
| `environmentName` | The name of the environment served for an assignment. |
| `userId` | A random Unity-generated GUID, uniquely identifying sessions played on the same instance of your application.<br><br>**Note**: The `userId` delivered in the report is a hash of the original ID. |
| `customUserId` | An optional developer-generated unique identifier for the user. For more information on implementing custom user IDs, see documentation on [Code integration](CodeIntegration.md).<br><br>**Note**: The `customUserId` delivered in the report is a hash of the original ID. |
| `assignmentId` | A unique ID generated when a player requests Remote Config settings from the service. |
| `ruleId` | The GUID identifying the Rule that is assigned to a user. |
| `ruleName` | The name of the Rule assigned to a user. |
| `variantId` | An identifier for an optional variant served for an assignment. |
| `ts` | A timestamp for the assignment in GMT. |

Note that the `userId` and `customUserId` are hashed, but can still be used to count unique user assignments.

If your game also sends user behavior events to Unity or your own analytics endpoint, you can cross reference this assignment summary report with a user behavior events report using the `assignmentId`.  

Please contact Unity to request an assignment report.
