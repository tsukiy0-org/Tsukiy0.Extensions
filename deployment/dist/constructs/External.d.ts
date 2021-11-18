import { Stack } from "aws-cdk-lib";
import { IVpc } from "aws-cdk-lib/lib/aws-ec2";
import { IGrantable } from "aws-cdk-lib/lib/aws-iam";
import { Construct } from "constructs";
export declare class External extends Construct {
    private readonly props;
    readonly vpc: IVpc;
    constructor(scope: Construct, id: string, props: {
        stack: Stack;
    });
    grantReadParam: (grantable: IGrantable, key: string) => void;
}
