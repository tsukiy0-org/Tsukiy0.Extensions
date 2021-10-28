import { Stack, StackProps } from "aws-cdk-lib";
import { Construct } from "constructs";
import { ExampleAspNetCoreApi } from "../constructs/ExampleAspNetCoreApi";

export class AppStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    new ExampleAspNetCoreApi(this, "ExampleAspNetCoreApi");
  }
}
