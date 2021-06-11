import { CfnOutput, Construct, Stack, StackProps } from "@aws-cdk/core";
import { SqsClient } from "./constructs/SqsClient";

export class Root extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    new SqsClient(this, "SqsClient");
  }
}
