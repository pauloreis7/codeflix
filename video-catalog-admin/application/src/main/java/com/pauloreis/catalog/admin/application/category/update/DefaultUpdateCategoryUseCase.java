package com.pauloreis.catalog.admin.application.category.update;

import java.util.Objects;
import java.util.function.Supplier;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.domain.exceptions.DomainException;
import com.pauloreis.catalog.admin.domain.validation.Error;
import com.pauloreis.catalog.admin.domain.validation.handler.Notification;

import io.vavr.control.Either;

import static io.vavr.API.Left;
import static io.vavr.API.Try;

public class DefaultUpdateCategoryUseCase extends UpdateCategoryUseCase {

  private CategoryGateway categoryGateway;

  DefaultUpdateCategoryUseCase(final CategoryGateway categoryGateway) {
    this.categoryGateway = Objects.requireNonNull(categoryGateway);
  }

  @Override
  public Either<Notification, UpdateCategoryOutput> execute(UpdateCategoryCommand aCommand) {
    final var anId = CategoryID.from(aCommand.id());
    final var aName = aCommand.name();
    final var aDescription = aCommand.description();
    final var isActive = aCommand.isActive();

    final var aCategory = categoryGateway.findById(anId).orElseThrow(notFound(anId));

    final var notification = Notification.create();

    aCategory.update(aName, aDescription, isActive).validate(notification);

    return notification.hasError() ? Left(notification) : update(aCategory);
  }

  private Either<Notification, UpdateCategoryOutput> update(final Category aCategory) {
    return Try(() -> categoryGateway.update(aCategory))
        .toEither()
        .bimap(Notification::create, UpdateCategoryOutput::from);
  }

  private Supplier<DomainException> notFound(final CategoryID anId) {
    final var anErrorMessage = String.format("Category with ID %s was not found", anId.getValue());

    return () -> DomainException.with(new Error(anErrorMessage));
  }
}