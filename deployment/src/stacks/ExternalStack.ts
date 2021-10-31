import { Stack, StackProps } from "aws-cdk-lib";
import { Construct } from "constructs";
import { TestTable } from "../constructs/TestTable";

export class ExternalStack extends Stack {
  constructor(
    scope: Construct,
    id: string,
    props: StackProps & {
      tableName: string;
    }
  ) {
    super(scope, id, props);

    new TestTable(this, "TestDynamoTable", {
      tableName: props.tableName,
    });
  }
}
