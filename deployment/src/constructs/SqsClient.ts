import { Queue } from "@aws-cdk/aws-sqs";
import { CfnOutput, Construct, Duration, RemovalPolicy } from "@aws-cdk/core";

export class SqsClient extends Construct {
  constructor(scope: Construct, id: string) {
    super(scope, id);

    const queue = new Queue(this, "Queue", {
      retentionPeriod: Duration.minutes(10),
      removalPolicy: RemovalPolicy.DESTROY,
    });

    const p = new CfnOutput(this, "SqsClientQueueUrl", {
      exportName: "SqsClientQueueUrl",
      value: queue.queueUrl,
    });
  }
}
