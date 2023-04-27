package com.pauloreis.catalog.admin.application.category.create;

import java.util.Objects;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.validation.handler.Notification;

public class DefaultCreateCategoryUseCase extends CreateCategoryUseCase {

  private final CategoryGateway categoryGateway;

  public DefaultCreateCategoryUseCase(final CategoryGateway categoryGateway) {
    this.categoryGateway = Objects.requireNonNull(categoryGateway);
  }

  @Override
  public CreateCategoryOutput execute(final CreateCategoryCommand aCommand) {
    final var aName = aCommand.name();
    final var aDescription = aCommand.description();
    final var isActive = aCommand.isActive();

    final var notification = Notification.create();

    final var aCategory = Category.newCategory(aName, aDescription, isActive);

    aCategory.validate(notification);

    final var createdCategory = categoryGateway.create(aCategory);

    return CreateCategoryOutput.from(createdCategory);
  }
}
