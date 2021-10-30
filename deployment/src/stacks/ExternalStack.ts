import { Stack, StackProps } from "aws-cdk-lib";
import { Construct } from "constructs";
import { TestDynamoTable } from "../constructs/TestDynamoTable"

export class ExternalStack extends Stack {
  constructor(scope: Construct, id: string, props: StackProps) {
    super(scope, id, props);

    new TestDynamoTable(this, "TestDynamoTable");
  }
}
