using System;

namespace Amido.Stacks.DynamoDB.Tests;

public class FakeMenu
{
	public FakeMenu() => Id = Guid.NewGuid();
	public FakeMenu(Guid id) => Id = id;

	public Guid Id { get; }
}
