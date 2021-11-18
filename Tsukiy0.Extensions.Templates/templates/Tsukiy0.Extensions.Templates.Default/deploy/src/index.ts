import { ExternalStack } from "./stacks/ExternalStack";
import { AppStack } from "./stacks/AppStack";
import { App } from "aws-cdk-lib";

const app = new App();

new ExternalStack(app, "External", {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT!,
    region: process.env.CDK_DEFAULT_REGION!,
  },
});

new AppStack(app, "App", {
  env: {
    account: process.env.CDK_DEFAULT_ACCOUNT!,
    region: process.env.CDK_DEFAULT_REGION!,
  },
});
