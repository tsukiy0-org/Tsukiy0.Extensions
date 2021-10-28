import { Stack, StackProps } from "aws-cdk-lib";
import { Construct } from "constructs";
import { ExampleAspNetCoreApi } from "../constructs/ExampleAspNetCoreApi";
import { External } from "../constructs/External";

export class AppStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    const external = new External(this, "External", {
      stack: this,
    });
    new ExampleAspNetCoreApi(this, "ExampleAspNetCoreApi", {
      external,
    });
  }
}
